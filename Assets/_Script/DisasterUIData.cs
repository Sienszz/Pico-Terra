using System;
public class DisasterUIData
{
    public string warningMessage;
    public string pollutionBarOff;
    public string pollutionBarOn;
    public string impactMessage;
    public int dialog;
    public string spiritName;

    public DisasterUIData(string warningMessage, string pollutionBarOff, string pollutionBarOn, string impactMessage, int dialog, string spiritName)
    {
        this.warningMessage = warningMessage;
        this.pollutionBarOff = pollutionBarOff;
        this.pollutionBarOn = pollutionBarOn;
        this.impactMessage = impactMessage;
        this.dialog = dialog;
        this.spiritName = spiritName;
    }
}
