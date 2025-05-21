using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class World
{
    [System.Serializable]
    public class WorldSettings
    {
        public enum WorldCreateStates { Meta, Generating, Generated, Updating }
        public enum WorldActiveStates { Halted, Running, Paused, Editmode }
        public string name;
        public int seed;
        public string generator;
        public int version;
        public int epochCreated;
        public int epochUpdated;
        //runtimes        
        public float elapsedTime;
        public int gameSpeed;
        public Vector3 worldSpawn;
        public bool debugOn;
   
        public int simulationDistance;
        public int gameTickSpeed;

        public System.DateTime createdOn;
        
        public WorldSettings(string name = "New World", int seed = -1, string gen = "default", int ver = -1)
        {
            this.name = name;
            this.seed = seed;
            generator = gen;
            version = ver;
            if (ver == -1) ver = 0;
            if (gen == "default") gen = "Default"; // Very Important!
            if (seed != -1) return;
            while (seed.ToString().Length < 256) 
            {
                string seedText = seed.ToString();
                int newInteger = Random.Range(1, 255);
                seedText = seedText + newInteger.ToString();
                seed = int.Parse(seedText);
            }
            return;
        }
    }

    //enums
    public WorldSettings.WorldCreateStates CreateState { get; private set; } = WorldSettings.WorldCreateStates.Meta;
    public WorldSettings.WorldActiveStates ActiveState { get; private set; } = WorldSettings.WorldActiveStates.Halted;

    //constants
    public WorldSettings settings;

    UnityEvent _gameTick;
    UnityEvent _runWorld;
    UnityEvent _stopWorld;
    UnityEvent _spawnPlayer;
    UnityEvent _despawnPlayer;
    UnityEvent _spawnEntity;
    UnityEvent _deSpawnEntity;
    UnityEvent _worldAccouncement;

    public World(WorldSettings worldSettings)
    {
        if (CreateState == WorldSettings.WorldCreateStates.Meta) FirstLoad(worldSettings);
    }
    public class BiomeInfo
    {
        private int _id;
        private string _name;
    }

    void FirstLoad(WorldSettings settings)
    {
        string txtSeed = settings.seed.ToString();

        // Set nice display info
        //seed = seed.ToString();
        var txtName = settings.name.ToString();
        var txtGenerator = settings.generator.ToString();
        var txtVersion = settings.version.ToString();

        // Track epoch world was first created and last updated
        settings.createdOn = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc); // Make this display nicer
        settings.epochCreated = (int)(System.DateTime.UtcNow - settings.createdOn).TotalSeconds;
        settings.epochUpdated = (int)(System.DateTime.UtcNow - settings.createdOn).TotalSeconds; // Update this again after world gen.

        // Defaults
        settings.elapsedTime = 0;
        settings.gameSpeed = 1;
        settings.worldSpawn = Vector3.zero;
        settings.debugOn = true;
        
        // Runtime Defaults
        settings.simulationDistance = 64; // Twice max player view distance
        settings.gameTickSpeed = 5;

        // Sooo.. 
        // Now we generate an empty world grid using perlin white noise ('generator') based on the world seed.
        // _generator = worldType, biomeSpread, worldLimits, makeStructures, spawnRange
        // assuming all defaults.
        // we have a random seed.
        // we have a spawn vector.
        // we want to generate an area twice the simulation distance that can be saved to disk.
        // we want the seed to be a base such that the same world is generated each time from the same seed.
        // the seed is a 256 digit integer number.
        // we're going to use this for 98% of world generation with small changes based on the name chosen.

        // we need to know the game data folder, create a folder based on world name to save game data.
        // create the folder and save the initial world generation settings to a file. create a date and name .txt files, too.

        // create a folder for the default world dimension

        // generate an empty integer grid to be filled .. x - e/w, y - u/d, z, n/s x -SimDistance+SimDistance, z-SimDistance, y-WorldLimits/2.
        // 'ChunkInfo' xz location, biomeId, .. Generated terrains data based off the seed then saved to disk.

        // now the world is started generating 
        CreateState = WorldSettings.WorldCreateStates.Generating;
        // Once all default chunks are created set stated to generated.
    }

    public void FixedLoop()
    {
        if (CreateState != WorldSettings.WorldCreateStates.Generated || ActiveState != WorldSettings.WorldActiveStates.Running) return;                

    }

    public void SetActiveState(WorldSettings.WorldActiveStates newState)
    {
        ActiveState = newState;
    }

    public void SetCreateState(WorldSettings.WorldCreateStates newState)
    {
        CreateState = newState;
    }
}


