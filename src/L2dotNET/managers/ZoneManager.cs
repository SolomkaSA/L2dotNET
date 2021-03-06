﻿using System;
using log4net;
using L2dotNET.world;

namespace L2dotNET.managers
{
    public class ZoneManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ZoneManager));

        private static volatile ZoneManager _instance;
        private static readonly object SyncRoot = new object();

        public void Initialize()
        {
            L2WorldRegion[,] worldRegions = L2World.Instance.GetWorldRegions();

            try
            {
                //XmlDocument doc = new XmlDocument();
                //int fileCounter = 0;
                //string[] xmlFilesArray = Directory.GetFiles(@"data\xml\zones\");
                //for (int i = 0; i < xmlFilesArray.Length; i++) { }
            }
            catch (Exception e)
            {
                Log.Error($"ZoneManager: {e.Message}");
            }

            //int size = 0;
        }

        public static ZoneManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ZoneManager();
                }

                return _instance;
            }
        }
    }
}