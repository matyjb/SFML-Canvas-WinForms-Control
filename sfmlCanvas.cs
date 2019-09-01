using System;
using System.ComponentModel;
using System.Windows.Forms;
using SFML.Graphics;
using SFML.System;

namespace WindowsFormsApp
{
    public delegate void InitEventHandler(RenderWindow window);
    public delegate void UpdateEventHandler(RenderWindow window, Time elapsedTime);
    public delegate void DrawEventHandler(RenderWindow window);
    public partial class SfmlCanvas : UserControl
    {
        public RenderWindow RendWind;

        public event InitEventHandler OnInit;
        public event UpdateEventHandler OnUpdate;
        public event DrawEventHandler OnDraw;

        protected virtual void Init()
        {
            OnInit?.Invoke(RendWind);
        }
        protected virtual void Update(Time elapsedTime)
        {
            OnUpdate?.Invoke(RendWind, elapsedTime);
        }
        protected virtual void Draw()
        {
            OnDraw?.Invoke(RendWind);
        }
        public SfmlCanvas()
        {
            InitializeComponent();
        }
        private void SfmlCanvas_Load(object sender, EventArgs e)
        {
            if (!renderLoopWorker.IsBusy)
                renderLoopWorker.RunWorkerAsync(Handle);
        }

        private void RenderLoopWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RendWind = new RenderWindow((IntPtr)e.Argument);
            Clock clock = new Clock();
            Init();
            while (RendWind.IsOpen)
            {
                Time elapsed = clock.ElapsedTime;
                clock.Restart();
                RendWind.DispatchEvents();
                RendWind.Clear(new Color(BackColor.R, BackColor.G, BackColor.B));
                Update(elapsed);
                Draw();
                RendWind.Display();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (RendWind == null)
                base.OnPaint(e);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (RendWind == null)
                base.OnPaintBackground(pevent);
        }
    }
}
