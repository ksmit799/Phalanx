using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using Phalanx.Net;

namespace Phalanx
{
    public class Phalanx : MBSubModuleBase
    {
        public static bool Host = true;
        public static Phalanx Instance;

        public NetworkManager NetManager;
        
        private Harmony _harmonyInstance;

        public Phalanx()
        {
            Instance = this;
        }
        
        public void ShowJoinGameInquiry()
        {
            InformationManager.ShowTextInquiry(new TextInquiryData(
                "Enter server address",
                "",
                true,
                true,
                "Connect",
                "Cancel",
                NetManager.JoinCoopGame,
                null)
            );
        }

        protected override void OnSubModuleLoad()
        {
            // Patch internal Bannerlord methods.
            // Try to minimize public API changes to keep compatibility with other mods.
            _harmonyInstance = new Harmony("com.bannerlord.phalanx");
            _harmonyInstance.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            base.OnSubModuleLoad();

            NetManager = new NetworkManager(this);

            Module.CurrentModule.AddInitialStateOption(new InitialStateOption(
                "JoinCoop",
                new TextObject("Join Co-op Game"),
                9990,
                ShowJoinGameInquiry,
                () => false)
            );
        }

        protected override void OnSubModuleUnloaded()
        {
            // Remove patches.
            _harmonyInstance.UnpatchAll(_harmonyInstance.Id);
            
            base.OnSubModuleUnloaded();
        }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            NetManager.Tick(dt);
        }
    }
}
