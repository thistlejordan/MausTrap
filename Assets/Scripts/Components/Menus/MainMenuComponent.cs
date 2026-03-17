namespace Assets.Scripts.Components
{
    public class MainMenuComponent : MenuComponent
    {
        public override void OpenMenu()
        {
            _cursor.Image.sprite = _cursorSprite;
            base.OpenMenu();
        }
    }
}
