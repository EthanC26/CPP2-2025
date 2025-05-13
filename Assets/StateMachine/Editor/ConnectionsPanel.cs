using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using UnityEngine;

namespace StateMachine.Editor
{
    /// <summary>
    /// Visual component that displays incoming and outgoing connections from the selected state.
    /// </summary>
    public class ConnectionsPanel : VisualElement
    {
        //Type enum - we have either an incoming or outgoing panel
        public enum PanelType { Incoming, Outgoing }
        private PanelType panelType;
        
        // UI elements
        private ScrollView connectionsScrollView;
        private Label emptyLabel;

        // Event for state selection
        public event Action<string> OnStateSelected;

        //constructor takes in a panel type
        public ConnectionsPanel(PanelType panelType = PanelType.Incoming)
        {
            this.panelType = panelType;
            string name = Enum.GetName(typeof(PanelType), panelType);
            // Set up styles
            AddToClassList("connections-panel");

            // Create header
            var header = new Label($"{name} Connections");
            header.AddToClassList("panel-header");
            Add(header);

            // Create scroll view for connections
            connectionsScrollView = new ScrollView();
            connectionsScrollView.AddToClassList("connections-scroll-view");
            connectionsScrollView.pickingMode = PickingMode.Position; // Allow picking on the scroll view
            connectionsScrollView.contentContainer.pickingMode = PickingMode.Position; // Allow picking on the content container
            Add(connectionsScrollView);

            //// >>> ADD PANEL-LEVEL TEST BUTTON START <<<
            //var panelTestButton = new Button(() => {
            //    Debug.Log($"PANEL TEST BUTTON in '{name} ConnectionsPanel' CLICKED!");
            //});
            //panelTestButton.text = "Test This Panel Button";
            //panelTestButton.style.backgroundColor = Color.green; // Different color for easy identification
            //panelTestButton.style.color = Color.white;
            //panelTestButton.style.paddingTop = 5;
            //panelTestButton.style.paddingBottom = 5;
            //panelTestButton.style.marginTop = 10;
            //panelTestButton.style.marginBottom = 10;
            //panelTestButton.pickingMode = PickingMode.Position;
            //this.Add(panelTestButton); // Add directly to the ConnectionsPanel (this)
            //                           // >>> ADD PANEL-LEVEL TEST BUTTON END <<<

            // Create empty label
            emptyLabel = new Label($"No {name.ToLower()} connections");
            emptyLabel.AddToClassList("empty-label");
            connectionsScrollView.Add(emptyLabel);
        }

        /// <summary>
        /// Updates the panel with connections from the selected state.
        /// </summary>
        /// <param name="transitions">The list of transitions from the selected state</param>
        /// <param name="graphData">The graph data</param>
        public void UpdateConnections<TContext>(
            List<StateMachineGraphData<TContext>.TransitionData> transitions,
            StateMachineGraphData<TContext> graphData) where TContext : class
        {
            // Clear existing connections
            connectionsScrollView.Clear();

            // Show empty label if no connections
            if (transitions == null || transitions.Count == 0)
            {
                connectionsScrollView.Add(emptyLabel);
                return;
            }

            // Add connection items
            foreach (var transition in transitions)
            {
                // Get target state - either incoming states or outgoing states based on panelType
                var targetState = graphData.GetStateNodeData((panelType == PanelType.Outgoing) ? transition.TargetStateId : transition.SourceStateId);
                if (targetState == null)
                    continue;

                // Create connection item
                var connectionItem = new VisualElement();
                connectionItem.AddToClassList("connection-item");
                //connectionItem.name = $"ConnectionItem_{targetState.Id}"; // For easier identification in debugger
                //                                                          // Temporarily remove USS class to rule out style conflicts for this test:
                //                                                          // connectionItem.AddToClassList("connection-item"); 
                //connectionItem.style.backgroundColor = Color.yellow; // Make parent very visible
                //connectionItem.style.minHeight = 50;                 // Ensure parent has height
                //connectionItem.style.marginBottom = 5;               // Space between items
                //connectionItem.style.flexGrow = 1;                   // Allow it to take space
                //connectionItem.pickingMode = PickingMode.Position;   // Explicitly pickable

                // Add status indicator
                var statusIndicator = new VisualElement();
                statusIndicator.AddToClassList("connection-indicator");

                // Set indicator class based on state status
                if (targetState.IsCurrentState)
                {
                    statusIndicator.AddToClassList("current-state");
                    // Only add active-connection class for outgoing transitions
                    if (panelType == PanelType.Outgoing)
                    {
                        connectionItem.AddToClassList("active-connection");
                    }
                }
                else if (targetState.IsPreviousState)
                {
                    statusIndicator.AddToClassList("previous-state");
                }
                else
                {
                    statusIndicator.AddToClassList("normal-state");
                }

                connectionItem.Add(statusIndicator);

                // Add state name
                var nameLabel = new Label(targetState.Name.ToUpper());
                nameLabel.AddToClassList("connection-name");
                connectionItem.Add(nameLabel);

                // Create transition sections container
                var transitionSections = new VisualElement();
                transitionSections.AddToClassList("transition-sections");

                // Add transition action section if it's an action transition

                var actionContainer = new VisualElement();
                actionContainer.AddToClassList("action-container");

                var actionLabel = new Label("Transition Action:");
                actionLabel.AddToClassList("action-label");
                actionContainer.Add(actionLabel);

                var actionValue = (transition.TransitionType.Contains("No")) ? new Label(transition.TransitionType) : new Label(transition.TransitionType.Split("\n")[1]);
                actionValue.AddToClassList("action-value");
                actionContainer.Add(actionValue);

                transitionSections.Add(actionContainer);


                // Add transition priority if available
                if (transition.Priority > 0)
                {
                    var priorityContainer = new VisualElement();
                    priorityContainer.AddToClassList("priority-container");

                    var priorityLabel = new Label("Priority:");
                    priorityLabel.AddToClassList("priority-label");
                    priorityContainer.Add(priorityLabel);

                    var priorityValue = new Label(transition.Priority.ToString());
                    priorityValue.AddToClassList("priority-value");
                    priorityContainer.Add(priorityValue);

                    transitionSections.Add(priorityContainer);
                }

                // Add transition condition if available
                if (!string.IsNullOrEmpty(transition.ConditionDescription))
                {
                    var conditionContainer = new VisualElement();
                    conditionContainer.AddToClassList("condition-container");

                    var conditionLabel = new Label("Condition:");
                    conditionLabel.AddToClassList("condition-label");
                    conditionContainer.Add(conditionLabel);

                    var conditionValue = new Label(transition.ConditionDescription);
                    conditionValue.AddToClassList("condition-value");
                    conditionContainer.Add(conditionValue);

                    transitionSections.Add(conditionContainer);
                }

                // Only add the sections container if it has children
                if (transitionSections.childCount > 0)
                {
                    connectionItem.Add(transitionSections);
                }

                var navigateLabelButton = new Label("Go to");
                navigateLabelButton.AddToClassList("navigate-button");
                navigateLabelButton.pickingMode = PickingMode.Position;

                navigateLabelButton.RegisterCallback<PointerDownEvent>(evt => {
                    //Debug.Log($"Label-as-button for {targetState.Name} (ID: {targetState.Id}) - PointerDownEvent! Invoking OnStateSelected.");
                    OnStateSelected?.Invoke(targetState.Id);
                    // evt.StopPropagation(); // Optional: can sometimes help if events are being unexpectedly propagated
                });
                connectionItem.Add(navigateLabelButton);

                // Add to scroll view
                connectionsScrollView.Add(connectionItem);
            }
        }
    }
}
