using System;
using System.Threading.Tasks;
using System.Timers;
using SensiEdge.Device;

namespace SensiEdgeDemo.Demo
{
    public interface INext<T>
    {
        T Next();
    }
    public class DemoDataSource<T> : ISource<T> where T : IParse
    {
        public bool IsAvailable => true;
        private INext<T> Next { get; set; }
        public event OnChange<T> OnChange;
        public DemoDataSource(INext<T> next)
        {
            Next = next;
            Timer = new Timer(1000);
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OnChange?.Invoke(Next.Next());
        }

        Timer Timer { get; set; }

        public void Disable()
        {
            Timer.Enabled = false;
        }

        public void Enable()
        {
            Timer.Enabled = true;
        }

        public Task<T> GetValue()
        {
            return Task.Factory.StartNew<T>(()=> { return Next.Next(); });
        }

        public void SetValue(T value) { }

        public void Dispose() { }
    }
}