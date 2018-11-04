/// <summary>
/// Usamos esta interfaz, para poder switchear entre armas desde cualquier Player.
/// </summary>
interface IWeapon
{
    void OnSelect();
    /// <summary>
    /// Activa el disparo del arma.
    /// </summary>
    void Shoot();
    /// <summary>
    /// Permite recargar el arma.
    /// </summary>
    void Reload();
}
