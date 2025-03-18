using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MathOperators;

public partial class MainPage : ContentPage
{
    /// <summary>
    /// Remembers the expressions that were calculated by the user
    /// </summary>
    private ObservableCollection<string> _expList;

    public MainPage()
    {
        InitializeComponent();
        _expList = new ObservableCollection<string>();
        _lstExpHistory.ItemsSource = _expList;
    }

    private async void OnCalculate(object sender, EventArgs e)
    {
        try
        {
            // Get the number inputs to operate on
            double leftOperand = double.Parse(_txtLeftOp.Text);
            double rightOperand = double.Parse(_txtRightOp.Text);

            // Get the specified operation type as a char
            // Cast to string is possible because SelectedItem is an object
            // Extra parenthesis are needed to ensure the index operator is applied to the string object, not the object
            char operation = ((string)_pckOperand.SelectedItem)[0];


            // Perform the operation to get the answer
            double result = PerformArithmeticOperation(operation, leftOperand, rightOperand);

            // Display the whole operation and answer text to the user
            string expression = $"{leftOperand} {operation} {rightOperand} = {result}";

            // Remember the expression in the page using the field variable list
            _expList.Add(expression);

            _txtMathExp.Text = expression;
        }
        catch (ArgumentNullException ex)
        {
            //The user did not provide any input
            await DisplayAlert("Arithmetic Calculator Error", "Please provide the required input", "OK");
            //await DisplayAlert("MORE", "More details about this error", "OK");

        }
        catch (FormatException ex)
        {
            //The user provided non-numeric input
            await DisplayAlert("Arithmetic Calculator Error", "Please provide the correct input", "OK");
        }
        catch (DivideByZeroException ex)
        {
            //The user tried to divide by zero (this only occurs for integer division, but not real division)
            await DisplayAlert("Arithmetic Calculator Error", "Please provide a non-zero denominator!", "OK");
        }
    }

    private double PerformArithmeticOperation(char operation, double leftOperand, double rightOperand)
    {
        switch (operation)
        {
            case '+':
                return PerformAddition(leftOperand, rightOperand);
            
            case '-':
                return PerformSubtraction(leftOperand, rightOperand);
            
            case '*':
                return PerformMultiplication(leftOperand, rightOperand);
            
            case '/':
                return PerformDivision(leftOperand, rightOperand);
            
            case '%':
                return PerformModulo(leftOperand, rightOperand);

            default:
                Debug.Assert(false, "Unknown arithmetic operand. Cannot perform the arithmetic operation.");
                return 0;
        }
    }

    private double PerformAddition(double leftOperand, double rightOperand)
    {
        return leftOperand + rightOperand;
    }
    
    private double PerformSubtraction(double leftOperand, double rightOperand)
    {
        return leftOperand - rightOperand;
    }
    
    private double PerformMultiplication(double leftOperand, double rightOperand)
    {
        return leftOperand * rightOperand;
    }
    
    private double PerformDivision(double leftOperand, double rightOperand)
    {
        string divOp = _pckOperand.SelectedItem as string; //another way of casting used for objects
        if (divOp.Contains("Int", StringComparison.OrdinalIgnoreCase))
        {
            //Integer division is performed when the operands are both integers
            int intLeftOp = (int)leftOperand;
            int intRightOp = (int)rightOperand;
            int result = intLeftOp / intRightOp;
            return result;
        }
        else
        {
            //Real division
            return leftOperand / rightOperand;
        }
    }
    
    private double PerformModulo(double leftOperand, double rightOperand)
    {
        return leftOperand % rightOperand;
    }
}