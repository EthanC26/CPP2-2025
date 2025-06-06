using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour
{
    // Save game data
    [XmlRoot("GameStateData")]
    public class GameStateData
    {
       public struct DataTransform
        {
            public float posx;
            public float posy;
            public float posz;
            public float rotx;
            public float roty;
            public float rotz;
            public float scaleX;
            public float scaleY;
            public float scaleZ;
        }
        // Data for player
        public class DataPlayer
        {
            //Transform Data
            public DataTransform transformData;
            //Has Collected Gun 01?
            public bool collectedWeapon;
            //Health
            public int health;
        }
        // Data for enemy
        public class DataEnemy
        {
            //Enemy Transform Data
            public DataTransform transformData;
            // Is Enemy Dead?
            public bool isDead;
            //Health
            public int health;
        }

        // Instance variables
        public DataPlayer playerData = new DataPlayer();
        public DataEnemy enemyData = new DataEnemy();
    }
    // Game data to save/load
    public GameStateData gameState = new GameStateData();
    // Saves game data to XML file
    public void Save(string fileName = "GameData.xml")
    {
        // Save game data
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream fileStream = new FileStream(fileName, FileMode.Create);
        serializer.Serialize(fileStream, gameState);

        fileStream.Flush();
        fileStream.Close();
        fileStream.Dispose();
    }
    // Load game data from XML file
    public void Load(string fileName = "GameData.xml")
    {

        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        gameState = serializer.Deserialize(fileStream) as GameStateData;

        fileStream.Flush();
        fileStream.Close();
        fileStream.Dispose();

    }
}
