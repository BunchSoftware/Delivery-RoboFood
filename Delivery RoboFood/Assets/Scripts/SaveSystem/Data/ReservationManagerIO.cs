using System;

public class ReservationManagerIO : DataStream
{
    private string _pathReservationFile;

    public ReservationManagerIO(string pathSaveFile)
    {
        _pathReservationFile = pathSaveFile;
    }

    public void CreateReservationPlayerData(PlayerData playerData)
    {
        if (playerData != null)
            base.Serialize(_pathReservationFile, playerData);
        else
            throw new Exception("Сохранения пусты");
    }
    public PlayerData LoadReservationPlayerData()
    {
        return base.Deserialize<PlayerData>(_pathReservationFile);
    }

    public void CreateReservationApplicationData(ApplicationData applicationData)
    {
        if (applicationData != null)
            base.Serialize(_pathReservationFile, applicationData);
        else
            throw new Exception("Сохранения пусты");
    }
    public ApplicationData LoadReservationApplicationData()
    {
        return base.Deserialize<ApplicationData>(_pathReservationFile);
    }

    public void Delete()
    {
        base.Delete(_pathReservationFile);
    }

    public void DeleteAll()
    {
        base.DeleteAll(_pathReservationFile);
    }
}
