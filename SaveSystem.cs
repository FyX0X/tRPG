using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Entity;

public static class SaveSystem
{

    public static void SavePlayer (Player player)
    {
        BinaryFormatter bf = new  BinaryFormatter();
        string path = @$"data\save\{player.name}.save";

        FileStream stream = new FileStream(path, FileMode.Create);
        bf.Serialize(stream, player);
        stream.Close();

    }

    public static Player LoadPlayer (string fileName)
    {
        
        BinaryFormatter bf = new  BinaryFormatter();
        string path = @$"data\save\{fileName}.save";
        FileStream stream = new FileStream(path, FileMode.Open);
        Player playerData = bf.Deserialize(stream) as Player;
        stream.Close();

        return playerData;

    }

}
