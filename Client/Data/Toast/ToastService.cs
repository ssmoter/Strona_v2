using System.Timers;

namespace Strona_v2.Client.Data.Toast
{
    public class ToastService : IDisposable
    {
        public event Action<string, ToastLevel>? OnShow;
        public event Action OnHide;
        private System.Timers.Timer CountDown;

        public void ShowToast(String message, ToastLevel level)
        {
            OnShow?.Invoke(message, level);
            StartCountDown();
        }

        private void StartCountDown()
        {
            SetCountDown();

            if (CountDown.Enabled)
            {
                CountDown.Stop();
                CountDown.Start();
            }
            else
            {
                CountDown.Start();
            }

        }

        private void SetCountDown()
        {
            if (CountDown == null)
            {
                CountDown = new System.Timers.Timer(5_000);
                CountDown.Elapsed += HideToast;
                CountDown.AutoReset = false;

            }
        }

        private void HideToast(object source,ElapsedEventArgs args)
        {
            OnHide?.Invoke();
        }

        public void Dispose()
        {
            CountDown?.Dispose();
        }
    }
}
