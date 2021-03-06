﻿using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using Tests.Factories;
using System.Tuples;

namespace Tests.Tmx
{
    [TestClass]
    public class LayerTests
    {
        private Layer _layer;

        [TestInitialize]
        public void Initialize()
        {
            _layer = TmxFactory.BuildMap().Layers[1];
        }

        [TestMethod]
        public void Layer_ConstructionUsingMinimalTmxFile_IsSuccessful()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElement(), 15, 15);

            Assert.AreEqual("Background", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are Tiles in the layer created?
            Assert.AreEqual(15 * 15, layer.Tiles.Count);
        }

        [TestMethod]
        public void Layer_ConstructionUsingLayerDataWithProperties_IsSuccessful()
        {
            Layer layer = new Layer(TmxFactory.BuildLayerXElementWithProperties(), 15, 15);

            Assert.AreEqual("Characters", layer.Name);
            Assert.AreEqual(15, layer.Width);
            Assert.AreEqual(15, layer.Height);
            Assert.AreEqual(1.0, layer.Opacity);
            Assert.AreEqual(true, layer.IsVisible);

            // Are Tiles in the layer created?
            Assert.AreEqual(8, layer.Tiles.Count);

            // Are the Properties for this Layer loaded?
            Assert.AreEqual(1, layer.Properties.Count);
        }
    }
}
