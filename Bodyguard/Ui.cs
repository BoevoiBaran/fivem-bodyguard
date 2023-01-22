using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LemonUI;
using LemonUI.Menus;

// ReSharper disable once CheckNamespace
namespace Client
{
    public enum UiState
    {
        Hide,
        Show
    }
    
    public class Ui : BaseScript
    {
        private readonly ObjectPool _pool;
        private readonly NativeMenu _menu;
        private readonly UiState _uiState;
        
        public Ui()
        {
            Tick += OnTick;
            
            _pool = new ObjectPool();
            _menu = new NativeMenu("Bodyguard menu", "Spawn bodyguard");
            _pool.Add(_menu);
            
            var item = new NativeItem("Example title");
            _menu.Add(item);

            _uiState = UiState.Hide;
        }

        private async Task OnTick()
        {
            _pool.Process();
            
            if(API.IsControlJustPressed(0, 47))
            {
                switch (_uiState)
                {
                    case UiState.Hide:
                        _menu.Visible = true;
                        break;
                    case UiState.Show:
                        _menu.Visible = false;
                        break;
                    default:
                        Debug.WriteLine("[Ui] Incorrect ui state");
                        break;
                }
            }
            
            await Task.FromResult(true);
        }
    }
}