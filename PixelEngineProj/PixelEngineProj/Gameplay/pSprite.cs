﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.Window;
using PixelEngine.System;

namespace PixelEngine.Gameplay {
    public class pSprite : Sprite{
        /// <summary>
        /// TODO: Compress this constructor, too many required params.
        /// </summary>
        /// <param name="texPath"></param>
        /// <param name="texRect"></param>
        /// <param name="position"></param>
        /// <param name="spriteRotation"></param>
        /// <param name="texRepeat"></param>
        public pSprite(String texPath, IntRect texRect, Vector2f position,float spriteRotation = 0, bool texRepeat = false){
            if (texPath != null) {
                //Init a new texture
                Texture newTex = new Texture(texPath, texRect);
                newTex.Update(new Image(texPath));
                //Init texture params
                Texture = newTex;
                Texture.Smooth = false;
                Texture.Repeated = texRepeat;

                //Assign sprite's loc and rot
                Rotation = spriteRotation;
                Position = position;
            }
        }

        public void Draw(RenderTarget target, RenderStates states) {
            base.Draw(target, new RenderStates(Texture));
        }
    }
}
