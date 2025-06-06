using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour
{
    private static readonly byte[] key = new byte[32]
    {
       1,2, 3, 4, 5, 6, 7, 8,
       9,10,11,12,13,14,15,16,
       17,18,19,20,21,22,23,24,
       25,26,27,28,29,30,31,32
    };

    private static readonly byte[] iv = new byte[16]
    {
       33,34,35,36,37,38,39,40,
       41,42,43,44,45,46,47,48
    };

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
        using FileStream fileStream = new FileStream(fileName, FileMode.Create);
        
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        serializer.Serialize(cryptoStream, gameState);
    }
    // Load game data from XML file
    public void Load(string fileName = "GameData.xml")
    {
      if(!File.Exists(fileName))
        {
            Debug.LogError($"Load failed: File {fileName} does not exist!");
            return;
        }

      XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
      using FileStream fileStream = new FileStream(fileName, FileMode.Open);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        gameState = (GameStateData)serializer.Deserialize(cryptoStream) as GameStateData;

    }
}