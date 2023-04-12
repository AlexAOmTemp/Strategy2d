using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class DataFilesLoader : MonoBehaviour
{
    public Dictionary<string, List<Sprite>> LoadedSpriteFiles { get; private set; } = new Dictionary<string, List<Sprite>>();
    public Dictionary<string,string> LoadedXMLFiles { get; private set; } = new Dictionary<string, string>();

    private void Awake()
    {
        LoadFiles();
    }
    public void LoadFiles ()
    {
        //Create an array of file paths from which to choose
        string spriteFolderPath = Application.streamingAssetsPath + "/Sprites/";  //Get path of folder
        string xmlFolderPath = Application.streamingAssetsPath + "/XMLs/";  //Get path of folder
        DirectoryInfo spritesFolder = new DirectoryInfo(spriteFolderPath);
        DirectoryInfo xmlFolder = new DirectoryInfo(xmlFolderPath);
        DirectoryInfo[] subDirs = spritesFolder.GetDirectories();
        FileInfo[] xmlFiles;
        xmlFiles = getFiles(xmlFolder, ".xml");
        foreach (FileInfo file in xmlFiles)
        {
            LoadedXMLFiles.Add (Path.GetFileNameWithoutExtension(file.Name), fileToString(file) );
        }


            foreach (DirectoryInfo dirInfo in subDirs)
        {
            FileInfo[] files = getFiles(dirInfo, ".png");
            List<Sprite> sprites = new List<Sprite>();
            foreach (FileInfo file in files)
                sprites.Add(fileToSprite(file));
            LoadedSpriteFiles.Add(dirInfo.Name, sprites);
        }
        consoleLogLoadedFiles();
    }
    private string fileToString(FileInfo file)
    {
        //Open file for Read\Write
        FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);

        //Create object of StreamReader by passing FileStream object on which it needs to operates on
        StreamReader streamReader = new StreamReader(stream);

        //Use ReadToEnd method to read all the content from file
        string fileContent = streamReader.ReadToEnd();

        //Close StreamReader object after operation
        streamReader.Close();
        stream.Close();
        return fileContent;
    }
    private Sprite fileToSprite(FileInfo file)
    {
        byte[] pngBytes = System.IO.File.ReadAllBytes(file.FullName);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(pngBytes);
        Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        fromTex.name = Path.GetFileNameWithoutExtension(file.Name);
        return fromTex;
    }
    private FileInfo[] getFiles(DirectoryInfo root, string fileExtension)
    {
        FileInfo[] files = null;
        try
        {
            files = root.GetFiles("*"+ fileExtension);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.LogError($"DataFilesLoader: Can't load {fileExtension} files from {root} no such directory: {e}");
        }
        if (files == null)
            Debug.LogError($"DataFilesLoader: Files {fileExtension} from {root} aren't loaded");
        return files;
    }
    private void consoleLogLoadedFiles()
    {
        Debug.Log("XML Files Loaded:");
        foreach (var file in LoadedXMLFiles)
            Debug.Log($"file {file.Key}");
        Debug.Log("PNG Files Loaded:");
        foreach (var pair in LoadedSpriteFiles)
        {
            Debug.Log($"From folder {pair.Key}");
            foreach (var sprite in LoadedSpriteFiles[pair.Key])
                Debug.Log($"Sprite {sprite.name}");
        }
    }
}


