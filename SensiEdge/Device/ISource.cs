using System;
using System.Threading.Tasks;

namespace SensiEdge.Device
{
    public delegate void OnChange<T>(T value);
    public interface IParse
    {
        void Parse(byte[] data);
        byte[] ToSetValue();
    }
    public interface ISource<T> : IDisposable where T : IParse
    {
        bool IsAvailable { get; }
        Task<T> GetValue();
        void SetValue(T value);
        void Enable();
        void Disable();
        event OnChange<T> OnChange;
    }
    public delegate void OnSubscribe();
    public interface ISubscribe
    {
        void Subscribe();
        void Unsubscribe();
        event OnSubscribe BeforeSubscribe;
        event OnSubscribe AfterUnsubscribe;
    } 
}