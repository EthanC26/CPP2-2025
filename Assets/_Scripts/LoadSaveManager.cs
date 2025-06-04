using System.Xml.Serialization;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour
{
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

        public class DataPlayer
        {
            public DataTransform transformData;

            public bool collectedWeapon;

            public int health;
        }

        public class DataEnemy
        {
            public DataTransform transformData;

            public bool isDead;

            public int health;
        }

        public DataPlayer playerData = new DataPlayer();
        public DataEnemy enemyData = new DataEnemy();
    }

    public GameStateData gameStateData = new GameStateData();

    public void Save()
    { }

    public void Load()
    { }
}
