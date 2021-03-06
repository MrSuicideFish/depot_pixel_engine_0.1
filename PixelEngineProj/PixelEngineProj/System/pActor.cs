﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace PixelEngine.System {
    public class pActor : pObject, Interfaces.IPlaceable{
        public static Vector2f position { get; set; }
        public static float rotation { get; set; }
        public static string SceneID, id;
        private pActor[] children;
        private pActor parent;

        public pActor(string ID = "Pixel Actor") {
            SceneID = ID;
        }

        public virtual void Begin() { }

        public virtual void Update() {
            Console.WriteLine("Updating " + SceneID);
        }

        public virtual void Draw(RenderTarget target, RenderStates states) {
            Console.WriteLine("Drawing " + SceneID);
        }

        public override string ToString(){
 	        return this.GetType().ToString() + ": " + id;
        }

        public pActor GetActorParent() {
            if (parent != null)
                return parent;
            else
                return null;
        }

        public pActor[] GetActorChildren() {
            if (children != null)
                return children;
            else
                return null;
        }
    }
}
