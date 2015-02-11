#region License
/*
Microsoft Public License (Ms-PL)
MonoGame - Copyright © 2009-2012 The MonoGame Team

All rights reserved.

This license governs use of the accompanying software. If you use the software,
you accept this license. If you do not accept the license, do not use the
software.

1. Definitions

The terms "reproduce," "reproduction," "derivative works," and "distribution"
have the same meaning here as under U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the
software.

A "contributor" is any person that distributes its contribution under this
license.

"Licensed patents" are a contributor's patent claims that read directly on its
contribution.

2. Grant of Rights

(A) Copyright Grant- Subject to the terms of this license, including the
license conditions and limitations in section 3, each contributor grants you a
non-exclusive, worldwide, royalty-free copyright license to reproduce its
contribution, prepare derivative works of its contribution, and distribute its
contribution or any derivative works that you create.

(B) Patent Grant- Subject to the terms of this license, including the license
conditions and limitations in section 3, each contributor grants you a
non-exclusive, worldwide, royalty-free license under its licensed patents to
make, have made, use, sell, offer for sale, import, and/or otherwise dispose of
its contribution in the software or derivative works of the contribution in the
software.

3. Conditions and Limitations

(A) No Trademark License- This license does not grant you rights to use any
contributors' name, logo, or trademarks.

(B) If you bring a patent claim against any contributor over patents that you
claim are infringed by the software, your patent license from such contributor
to the software ends automatically.

(C) If you distribute any portion of the software, you must retain all
copyright, patent, trademark, and attribution notices that are present in the
software.

(D) If you distribute any portion of the software in source code form, you may
do so only under this license by including a complete copy of this license with
your distribution. If you distribute any portion of the software in compiled or
object code form, you may only do so under a license that complies with this
license.

(E) The software is licensed "as-is." You bear the risk of using it. The
contributors give no express warranties, guarantees or conditions. You may have
additional consumer rights under your local laws which this license cannot
change. To the extent permitted under your local laws, the contributors exclude
the implied warranties of merchantability, fitness for a particular purpose and
non-infringement.
*/
#endregion License

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Tests.Components {
	class Two3DCubesComponent : VisualTestDrawableGameComponent {
		BasicEffect basicEffect;

		Matrix worldMatrix, viewMatrix, projectionMatrix;

        public Two3DCubesComponent(Game game)
            : base(game)
		{
		}

		protected override void LoadContent ()
		{
			base.LoadContent();

			// setup our graphics scene matrices
			worldMatrix = Matrix.Identity;
			viewMatrix = Matrix.CreateLookAt (new Vector3 (0, 0, 5), Vector3.Zero, Vector3.Up);
			projectionMatrix = Matrix.CreatePerspectiveFieldOfView (MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 10);

			worldMatrix *= Matrix.CreateRotationX (-0.05f * 30f);
			worldMatrix *= Matrix.CreateRotationY (-0.05f * 20f);

			// Setup our basic effect
			basicEffect = new BasicEffect (GraphicsDevice);
			basicEffect.World = worldMatrix;
			basicEffect.View = viewMatrix;
			basicEffect.Projection = projectionMatrix;

			CreateCubeVertexBuffer ();
			CreateCubeIndexBuffer ();
		}

		protected override void UnloadContent ()
		{
			indices.Dispose ();
			indices = null;
			vertices.Dispose ();
			vertices = null;

			base.UnloadContent ();
		}

		public override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			GraphicsDevice.SetVertexBuffer (vertices);
			GraphicsDevice.Indices = indices;

			//RasterizerState rasterizerState1 = new RasterizerState ();
			//rasterizerState1.CullMode = CullMode.None;
			//graphics.GraphicsDevice.RasterizerState = rasterizerState1;

			basicEffect.World = worldMatrix;
            basicEffect.DiffuseColor = Color.Red.ToVector3();

			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes) {
                pass.Apply();

				GraphicsDevice.DrawIndexedPrimitives (PrimitiveType.TriangleList, 0, 0,
					number_of_vertices, 0, number_of_indices / 3);
			}

            var secondWorldMatrix = worldMatrix;
            secondWorldMatrix *= Matrix.CreateTranslation(0.4f, 0, 0);
            basicEffect.World = secondWorldMatrix;
            basicEffect.DiffuseColor = Color.Green.ToVector3();

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                    number_of_vertices, 0, number_of_indices / 3);
            }

			base.Draw (gameTime);
		}

		const int number_of_vertices = 8;
		const int number_of_indices = 36;
		VertexBuffer vertices;

		void CreateCubeVertexBuffer ()
		{
            VertexPositionTexture[] cubeVertices = new VertexPositionTexture[number_of_vertices];

			cubeVertices [0].Position = new Vector3 (-1, -1, -1);
			cubeVertices [1].Position = new Vector3 (-1, -1, 1);
			cubeVertices [2].Position = new Vector3 (1, -1, 1);
			cubeVertices [3].Position = new Vector3 (1, -1, -1);
			cubeVertices [4].Position = new Vector3 (-1, 1, -1);
			cubeVertices [5].Position = new Vector3 (-1, 1, 1);
			cubeVertices [6].Position = new Vector3 (1, 1, 1);
			cubeVertices [7].Position = new Vector3 (1, 1, -1);

            vertices = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, number_of_vertices, BufferUsage.WriteOnly);
			vertices.SetData(cubeVertices);
		}

		IndexBuffer indices;

		void CreateCubeIndexBuffer ()
		{
			UInt16[] cubeIndices = new UInt16[number_of_indices];

			//bottom face
			cubeIndices [0] = 0;
			cubeIndices [1] = 2;
			cubeIndices [2] = 3;
			cubeIndices [3] = 0;
			cubeIndices [4] = 1;
			cubeIndices [5] = 2;

			//top face
			cubeIndices [6] = 4;
			cubeIndices [7] = 6;
			cubeIndices [8] = 5;
			cubeIndices [9] = 4;
			cubeIndices [10] = 7;
			cubeIndices [11] = 6;

			//front face
			cubeIndices [12] = 5;
			cubeIndices [13] = 2;
			cubeIndices [14] = 1;
			cubeIndices [15] = 5;
			cubeIndices [16] = 6;
			cubeIndices [17] = 2;

			//back face
			cubeIndices [18] = 0;
			cubeIndices [19] = 7;
			cubeIndices [20] = 4;
			cubeIndices [21] = 0;
			cubeIndices [22] = 3;
			cubeIndices [23] = 7;

			//left face
			cubeIndices [24] = 0;
			cubeIndices [25] = 4;
			cubeIndices [26] = 1;
			cubeIndices [27] = 1;
			cubeIndices [28] = 4;
			cubeIndices [29] = 5;

			//right face
			cubeIndices [30] = 2;
			cubeIndices [31] = 6;
			cubeIndices [32] = 3;
			cubeIndices [33] = 3;
			cubeIndices [34] = 6;
			cubeIndices [35] = 7;

			indices = new IndexBuffer (GraphicsDevice, IndexElementSize.SixteenBits, number_of_indices, BufferUsage.WriteOnly);
			indices.SetData<UInt16> (cubeIndices);

		}
	}
}
