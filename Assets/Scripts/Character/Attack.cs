

public class Attack : IBehavior
{
    private Gun currentGun;

    public Gun CurrentGun { get => currentGun; set => currentGun = value; }

    public void SetBehavior()
    {
        //Attack
    }

}
