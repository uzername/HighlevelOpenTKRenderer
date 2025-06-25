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
            exampleObject.UniqueName = "transparentCube";
            exampleObject.LitMaterial.DiffuseColor = new Vector4(0.5f, 0.7f, 0.6f, 0.5f);
            SceneObjects.Add(exampleObject);
            SceneObjects[SceneObjects.Count-1].MoveTo(0, 0, -3);

            // central cube at zero point. This one is hidden
            MeshConstructor3D.GetCubeMeshWithNormals(0.5f, out testcubeVertices, out testcubeIndices);
            LitObject3D central = new LitObject3D(testcubeVertices, testcubeIndices);
            central.UniqueName = "centralCube";
            central.IsShown = true;
            SceneObjects.Add(central);

            // simple object at lightsource
            uint[] testcubeIndices0 = new uint[] { };
            float[] testcubeVertices0 = new float[] { };
            MeshConstructor3D.GetCubeMesh(0.5f, out testcubeVertices0, out testcubeIndices0);
            SimpleObject3D lightcube = new SimpleObject3D(testcubeVertices0, testcubeIndices0);
            lightcube.UniqueName = "lightCube";
            lightcube.IsShown = true;
            SceneObjects.Add(lightcube);
            SceneObjects[SceneObjects.Count - 1].MoveTo(3.0f, 3.0f, 3.0f);

            // main grid
            uint[] testgridIndices = new uint[] { };
            float[] testgridVertices = new float[] { };
            MeshConstructor3D.GetSimpleGridMesh(50.0f, 50.0f, 50, 50, out testgridVertices, out testgridIndices);
            SimpleObject3D simpleGrid= new SimpleObject3D(testgridVertices, testgridIndices);
            simpleGrid.UniqueName = "mainGrid";
            simpleGrid.SimpleColor = new Vector4(0.75f, 0.75f, 0.75f,1.0f);
            simpleGrid.DrawTriangles = false;
            SceneObjects.Add(simpleGrid);

            // smooth sphere
            uint[] testSphereIndices = new uint[] { };
            float[] testSphereVertices = new float[] { };
            MeshConstructor3D.getSphereMeshWithNormals(2.0f, 2, out testSphereVertices, out testSphereIndices, true);
            LitObject3D sphericObject1 = new LitObject3D(testSphereVertices, testSphereIndices);
            sphericObject1.UniqueName = "sphere1";
            sphericObject1.IsShown = true;
            sphericObject1.DrawTriangles = true;
            sphericObject1.LitMaterial.DiffuseColor = new Vector4(0.25f, 0.75f, 0.4f, 1.0f);
            SceneObjects.Add(sphericObject1);
            SceneObjects[SceneObjects.Count - 1].MoveTo(0, 3.0f, -3.0f);

            // not smooth sphere
            uint[] testSphereIndices2 = new uint[] { };
            float[] testSphereVertices2 = new float[] { };
            MeshConstructor3D.getSphereMeshWithNormals(2.0f, 2, out testSphereVertices2, out testSphereIndices2, false);
            LitObject3D sphericObject2 = new LitObject3D(testSphereVertices2, testSphereIndices2);
            sphericObject2.UniqueName = "sphere2";
            sphericObject2.IsShown = true;
            sphericObject2.DrawTriangles = true;
            sphericObject2.LitMaterial.DiffuseColor = new Vector4(0.75f, 0.15f, 0.5f, 1.0f);
            SceneObjects.Add(sphericObject2);
            SceneObjects[SceneObjects.Count - 1].MoveTo(-3.0f, 0f, -3f);
        }
    }
}
