using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dailyTyping
{
    internal class Hook
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;

        private delegate IntPtr LowLevelKeyProc(int nCode, IntPtr wParam, IntPtr lParam);
        //이 위로는 DLL import 등입니다.

        private static LowLevelKeyProc keyboardProc = KeyboardHookProc;

        private static IntPtr keyHook = IntPtr.Zero;

        private static Queue<char> texts = new Queue<char>();

        private static int count = 0;

        private static Label templabel;

        private static long time1 = 0;

        private static long time2 = 0;

        private static char val1;

        private static char val2;

        public static void SetHook(IntPtr val, Label label)
        {

            if (keyHook == val)
            {
                count = 0;
                templabel = label;
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    keyHook = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        public static void UnHook()
        {
            UnhookWindowsHookEx(keyHook);
        }

        private static IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (time1 == 0)
            {               
                val1 = Convert.ToChar(Marshal.ReadInt32(lParam));
                time1 = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            }

            if(time1 > 0)
            {
                val2 = val1;
                time2 = time1;

                val1 = Convert.ToChar(Marshal.ReadInt32(lParam));
                time1 = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            }
            
            //Code가 0보다 클 때에만 처리해야 합니다. 아닌 경우에는 내부적으로 관리하는 훅체인에 쓰이기 때문입니다.
            //wParam==WM_KEYDOWN부분은, 키보드를 누르는 이벤트와 떼는 이벤트 중 누르는 이벤트만을 통과시킵니다.
            //만약 ==267로 바꿀 경우, 키보드를 땔 떼 코드가 실행됩니다.
            if (code >= 0 && (int)wParam == WM_KEYDOWN)
            {
                Debug.WriteLine("time1 : " + time1);
                Debug.WriteLine("val1 : " + val1);
                Debug.WriteLine("time2 : " + time2);
                Debug.WriteLine("val2 : " + val2);
                Debug.WriteLine("time1 - time2 : " + (time1 - time2));

                if (time2 > 0 && (time1 - time2 < 1 && val1 == val2))
                {
                    return CallNextHookEx(keyHook, code, (int)wParam, lParam);
                }
                
                

                //lParam포인터가 가리키는 곳에서 키코드를 읽어 keyCheck로 보냅니다.
                texts.Enqueue(Convert.ToChar(Marshal.ReadInt32(lParam)));//texts큐에 데이터를 집어넣음
                
                count += 1;
                templabel.Text = Convert.ToString(count);
                //return (IntPtr)1; <- 키를 씹을 수 있음
            }
            return CallNextHookEx(keyHook, code, (int)wParam, lParam);
        }

    }
}
