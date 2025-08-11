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
        
        public FirstPersonCamera camera;
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
            exampleObject.MoveTo(0, 0, -3);
            SceneObjects.Add(exampleObject);

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
            lightcube.MoveTo(3.0f, 3.0f, 3.0f);
            SceneObjects.Add(lightcube);

            // main grid
            uint[] testgridIndices = new uint[] { };
            float[] testgridVertices = new float[] { };
            MeshConstructor3D.GetSimpleGridMesh(50.0f, 50.0f, 50, 50, out testgridVertices, out testgridIndices);
            SimpleObject3D simpleGrid= new SimpleObject3D(testgridVertices, testgridIndices);
            simpleGrid.UniqueName = "mainGrid";
            simpleGrid.SimpleColor = new Vector4(0.75f, 0.75f, 0.75f,1.0f);
            simpleGrid.DrawTriangles = false;
            SceneObjects.Add(simpleGrid);

            //line of fixed width
            uint[] testLineIndices = new uint[] { };
            float[] testLineVertices = new float[] { };
            MeshConstructor3D.GetSingleLineMesh(10f, 0f, 0f, 5f, 10f, 5f, out testLineVertices, out testLineIndices);
            ThickLineObject3D thickLine = new ThickLineObject3D(testLineVertices, testLineIndices);
            thickLine.UniqueName = "thickLine1";
            thickLine.thicknessLine = 5.0f;
            thickLine.SimpleColor = new Vector4(0.95f, 0.3f, 0.05f, 1.0f);
            thickLine.DrawTriangles = false;
            SceneObjects.Add(thickLine);

            // shaded box
            uint[] testBoxIndices = new uint[] { };
            float[] testBoxVertices = new float[] { };
            MeshConstructor3D.GetBoxMeshWithNormals(4.0f, 1.0f, 0.5f, out testBoxVertices, out testBoxIndices);
            LitObject3D boxObject = new LitObject3D(testBoxVertices, testBoxIndices);
            boxObject.UniqueName = "box1";
            boxObject.IsShown = true;
            boxObject.LitMaterial.DiffuseColor = new Vector4(1.0f, 0.0f, 0.05f, 1.0f);
            boxObject.MoveTo(-1.0f, 0.0f, -5f);
            SceneObjects.Add(boxObject);

            // smoothed cylinder
            uint[] testCylinderIndices = new uint[] { };
            float[] testCylinderVertices = new float[] { };
            MeshConstructor3D.GetCylinderMeshWithNormals(2.0f,3.0f, out testCylinderVertices, out testCylinderIndices, true);
            LitObject3D cylObject = new LitObject3D(testCylinderVertices, testCylinderIndices);
            cylObject.UniqueName = "cylinder1";
            cylObject.IsShown = true;
            cylObject.LitMaterial.DiffuseColor = new Vector4(0.1f, 0.2f, 1.0f, 1.0f);
            cylObject.MoveTo(3.0f, 4.0f, -3f);
            SceneObjects.Add(cylObject);
            // not smoothed cylinder
            MeshConstructor3D.GetCylinderMeshWithNormals(2.0f, 1.0f, out testCylinderVertices, out testCylinderIndices, false);
            LitObject3D cylObject2 = new LitObject3D(testCylinderVertices, testCylinderIndices);
            cylObject2.UniqueName = "cylinder2";
            cylObject2.IsShown = true;
            cylObject2.LitMaterial.DiffuseColor = new Vector4(0.1f, 0.85f, 0.45f, 1.0f);
            cylObject2.MoveTo(-3.0f, 4.0f, -3f);
            SceneObjects.Add(cylObject2);

            // smooth sphere
            uint[] testSphereIndices = new uint[] { };
            float[] testSphereVertices = new float[] { };
            MeshConstructor3D.getSphereMeshWithNormals(2.0f, 2, out testSphereVertices, out testSphereIndices, true);
            LitObject3D sphericObject1 = new LitObject3D(testSphereVertices, testSphereIndices);
            sphericObject1.UniqueName = "sphere1";
            sphericObject1.IsShown = true;
            sphericObject1.DrawTriangles = true;
            sphericObject1.LitMaterial.DiffuseColor = new Vector4(0.25f, 0.75f, 0.4f, 1.0f);
            sphericObject1.MoveTo(0, 3.0f, -3.0f);
            SceneObjects.Add(sphericObject1);

            // not smooth sphere
            uint[] testSphereIndices2 = new uint[] { };
            float[] testSphereVertices2 = new float[] { };
            MeshConstructor3D.getSphereMeshWithNormals(2.0f, 2, out testSphereVertices2, out testSphereIndices2, false);
            LitObject3D sphericObject2 = new LitObject3D(testSphereVertices2, testSphereIndices2);
            sphericObject2.UniqueName = "sphere2";
            sphericObject2.IsShown = true;
            sphericObject2.DrawTriangles = true;
            sphericObject2.LitMaterial.DiffuseColor = new Vector4(0.75f, 0.15f, 0.5f, 1.0f);
            sphericObject2.MoveTo(-3.0f, 0f, -3f);
            SceneObjects.Add(sphericObject2);

            // not lit sphere
            uint[] testSphereIndices3 = new uint[] { };
            float[] testSphereVertices3 = new float[] { };
            MeshConstructor3D.getSphereMesh(2.0f, out testSphereVertices3, out testSphereIndices3);
            SimpleObject3D sphericObject3 = new SimpleObject3D(testSphereVertices3, testSphereIndices3);
            sphericObject3.UniqueName = "sphere3";
            sphericObject3.IsShown = true;
            sphericObject3.DrawTriangles = true;
            sphericObject3.SimpleColor = new Vector4(0.15f, 0.25f, 0.75f, 1.0f);
            sphericObject3.MoveTo(1f, 1f, 1f);
            SceneObjects.Add(sphericObject3);
        }
    }
}
