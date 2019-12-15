using System;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    public interface IEventSource : IDisposable
    {
        /// <summary>
        /// ユーザーが設定したオーバーレイの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// オーバーレイの更新を開始します。
        /// </summary>
        void Start();

        /// <summary>
        /// オーバーレイの更新を停止します。
        /// </summary>
        void Stop();

        Control CreateConfigControl();

        void LoadConfig(IPluginConfig config);

        void SaveConfig(IPluginConfig config);
    }
}
