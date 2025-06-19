using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public interface ICameraControl  {
        
        public void MoveUp();
        public void MoveDown();
        public void MoveLeft();
        public void MoveRight();
        public void MoveForward();
        public void MoveBackward();
        public void ProcessMouseInputLook(int cX, int cY);
        public void ProcessMouseScroll(int de);
    }
}
