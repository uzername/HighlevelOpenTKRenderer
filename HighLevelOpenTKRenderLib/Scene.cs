using OpenTK.Mathematics;

namespace HighLevelOpenTKRenderLib
{
    public class Scene  {
        public Color4 color1 = new Color4 { R = 0.2f, G = 0.7f, B = 0.9f, A = 1.0f };
        public Color4 color2 = new Color4 { R = 1.0f, G = 1.0f, B = 1.0f, A = 1.0f };
        
        /*
        public List<Light> SceneLights;
        public Light AmbientLight;
        public List<Object3D> SceneObjects;
        */
        public SimpleCamera camera;
        public SimpleObject3D object3D;
        public Scene() { 
            /*
            SceneLights = new List<Light>();
            SceneObjects = new List<Object3D>();
            */
            camera = new SimpleCamera();
            uint[] testcubeIndices = new uint[]   {
            0, 1, 5,  5, 1, 6,
            1, 2, 6,  6, 2, 7,
            2, 3, 7,  7, 3, 8,
            3, 4, 8,  8, 4, 9,
            10,11, 0,  0,11, 1,
            5, 6,12, 12, 6,13
            };
            float[] testcubeVertices = new float[] {
                   -1,-1,-1, 
                    1,-1,-1, 
                    1, 1,-1, 
                   -1, 1,-1, 
                   -1,-1,-1, 
                   -1,-1, 1, 
                    1,-1, 1, 
                    1, 1, 1, 
                   -1, 1, 1, 
                   -1,-1, 1, 
                   -1, 1,-1, 
                    1, 1,-1, 
                   -1, 1, 1, 
                    1, 1, 1, 
            };
            object3D = new SimpleObject3D(testcubeVertices, testcubeIndices);
        }
    }
}
