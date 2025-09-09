using UnityEngine;

public class CustomConfirm : MonoBehaviour
{
    public Color ErrorColor;

    [Header("Level")]
    public ToggleSwitch LevelToggle;
    public SettingsNumberBox LevelInput;
    public SettingsNumberBox PuzzleInput;

    [Header("By Type")]
    public ToggleSwitch SquareToggle;
    public SettingsNumberBox SquareInputMin;
    public SettingsNumberBox SquareInputMax;
    public ToggleSwitch DiamondToggle;
    public SettingsNumberBox DiamondInputMin;
    public SettingsNumberBox DiamondInputMax;
    public ToggleSwitch HexagonToggle;
    public SettingsNumberBox HexagonInputMin;
    public SettingsNumberBox HexagonInputMax;
    public ToggleSwitch RookToggle;
    public SettingsNumberBox RookInputMin;
    public SettingsNumberBox RookInputMax;

    protected Color defaultColor;

    protected bool allValid;


    protected virtual void Awake()
    {
        GameEventSystem.UI_CustomNumberBoxUpdate += GameEventSystem_UI_CustomNumberBoxUpdate;

        defaultColor = LevelInput.InputFieldValue.targetGraphic.color;
    }

    protected virtual void GameEventSystem_UI_CustomNumberBoxUpdate()
    {
        ValidateInputs();
    }

    public virtual void ConfirmClick()
    {
        ValidateInputs();
        if (!allValid) return;

        CustomValues.ResetValues();

        if (LevelToggle.CurrentValue)
        {
            CustomValues.Level = LevelInput.NumberBoxValue;
            CustomValues.Puzzle = PuzzleInput.NumberBoxValue;
        }
        else
        {
            if (SquareToggle.CurrentValue)
            {
                CustomValues.Square = true;
                CustomValues.MaxSquare = SquareInputMax.NumberBoxValue;
                CustomValues.MinSquare = SquareInputMin.NumberBoxValue;
            }
            if (DiamondToggle.CurrentValue)
            {
                CustomValues.Diamond = true;
                CustomValues.MaxDiamond = DiamondInputMax.NumberBoxValue;
                CustomValues.MinDiamond = DiamondInputMin.NumberBoxValue;
            }
            if (HexagonToggle.CurrentValue)
            {
                CustomValues.Hexagon = true;
                CustomValues.MaxHexagon = HexagonInputMax.NumberBoxValue;
                CustomValues.MinHexagon = HexagonInputMin.NumberBoxValue;
            }
            if (RookToggle.CurrentValue)
            {
                CustomValues.Rook = true;
                CustomValues.MaxRook = RookInputMax.NumberBoxValue;
                CustomValues.MinRook = RookInputMin.NumberBoxValue;
            }
        }

        GameEventSystem.Game_OnStart(MapType.Custom);
        GameEventSystem.UI_OnCustomConfirm();
    }

    protected virtual void ValidateInputs()
    {
        allValid = true;
        
        // no validation needed for level
        if (LevelToggle.CurrentValue)
        {
            if (LevelInput.NumberBoxValue < 1)
            {
                LevelInput.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else
            {
                LevelInput.InputFieldValue.targetGraphic.color = defaultColor;
            }

            if (PuzzleInput.NumberBoxValue > PuzzlesPerLevel.GetMaxPuzzleCountForLevel(LevelInput.NumberBoxValue))
            {
                PuzzleInput.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else
            {
                PuzzleInput.InputFieldValue.targetGraphic.color = defaultColor;
            }

            return;
        }

        if (SquareToggle.CurrentValue)
        {
            if (SquareInputMin.NumberBoxValue > SquareInputMax.NumberBoxValue)
            {
                SquareInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                SquareInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (SquareInputMin.NumberBoxValue < 1)
            {
                SquareInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (SquareInputMax.NumberBoxValue < 1)
            {
                SquareInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else
            {
                SquareInputMin.InputFieldValue.targetGraphic.color = defaultColor;
                SquareInputMax.InputFieldValue.targetGraphic.color = defaultColor;
            }
        }

        if (DiamondToggle.CurrentValue)
        {
            if (DiamondInputMin.NumberBoxValue > DiamondInputMax.NumberBoxValue)
            {
                DiamondInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                DiamondInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (DiamondInputMin.NumberBoxValue < 1)
            {
                DiamondInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (DiamondInputMax.NumberBoxValue < 1)
            {
                DiamondInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else
            {
                DiamondInputMin.InputFieldValue.targetGraphic.color = defaultColor;
                DiamondInputMax.InputFieldValue.targetGraphic.color = defaultColor;
            }
        }

        if (HexagonToggle.CurrentValue)
        {
            if (HexagonInputMin.NumberBoxValue > HexagonInputMax.NumberBoxValue)
            {
                HexagonInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                HexagonInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (HexagonInputMin.NumberBoxValue < 1)
            {
                HexagonInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (HexagonInputMax.NumberBoxValue < 1)
            {
                HexagonInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else
            {
                HexagonInputMin.InputFieldValue.targetGraphic.color = defaultColor;
                HexagonInputMax.InputFieldValue.targetGraphic.color = defaultColor;
            }
        }

        if (RookToggle.CurrentValue)
        {
            if (RookInputMin.NumberBoxValue > RookInputMax.NumberBoxValue)
            {
                RookInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                RookInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (RookInputMin.NumberBoxValue < 1)
            {
                RookInputMin.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else if (RookInputMax.NumberBoxValue < 1)
            {
                RookInputMax.InputFieldValue.targetGraphic.color = ErrorColor;
                allValid = false;
            }
            else
            {
                RookInputMin.InputFieldValue.targetGraphic.color = defaultColor;
                RookInputMax.InputFieldValue.targetGraphic.color = defaultColor;
            }
        }

    }

}
