using OpenTK.Mathematics;

namespace HighLevelOpenTKRenderLib
{
    public class Scene  {
        /// <summary>
        /// one color of scene gradient background
        /// </summary>
        public Color4 color1 = new Color4 { R = 0.2f, G = 0.7f, B = 0.9f, A = 1.0f };
        /// <summary>
        /// second color of scene gradient background
        /// </summary>
        public Color4 color2 = new Color4 { R = 1.0f, G = 1.0f, B = 1.0f, A = 1.0f };

        
        public List<Light> SceneLights;
        public Light AmbientLight;
        public List<Object3D> SceneObjects;
        
        public OrbitingCamera camera;
        public SimpleObject3D object3D;
        public Scene() {
            SceneLights = new List<Light>();
            SceneObjects = new List<Object3D>();
            
            
        }

        public void AddTestObject2()
        {
            uint[] testcubeIndices = new uint[] { };
            float[] testcubeVertices = new float[] { };
            MeshConstructor3D.GetCubeMeshWithNormals(2.0f, out testcubeVertices, out testcubeIndices);
            LitObject3D exampleObject = new LitObject3D(testcubeVertices, testcubeIndices);
            exampleObject.LitMaterial.DiffuseColor = new Vector4(0.5f, 0.7f, 0.6f, 0.5f);
            SceneObjects.Add(exampleObject);
            SceneObjects[SceneObjects.Count-1].MoveTo(0, 0, -3);

            MeshConstructor3D.GetCubeMeshWithNormals(0.5f, out testcubeVertices, out testcubeIndices);
            LitObject3D central = new LitObject3D(testcubeVertices, testcubeIndices);
            SceneObjects.Add(central);

            uint[] testgridIndices = new uint[] { };
            float[] testgridVertices = new float[] { };
            MeshConstructor3D.GetSimpleGridMesh(50.0f, 50.0f, 50, 50, out testgridVertices, out testgridIndices);
            SimpleObject3D simpleGrid= new SimpleObject3D(testgridVertices, testgridIndices);
            simpleGrid.SimpleColor = new Vector4(0.25f);
            simpleGrid.DrawTriangles = false;
            SceneObjects.Add(simpleGrid);
        }
    }
}
