namespace PersonalApi.DTOs;

public class MitarbeiterSuchParameterDto
{
    public string Suchbegriff { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Sortierfeld { get; set; } = "Mitarbeiterid";
    public bool Aufteigend { get; set; } = true;

    private int _seite;
    public int Seite 
    {  
        get => _seite;
     
        set => _seite = (value < 1 ) ? 1 : value;        
    }

    private int _seitenGroesse;
    public int SeitenGroesse 
    {
        get => _seitenGroesse;

        set => _seitenGroesse = (value > 50) ? 50 : value;

    
    }
}
