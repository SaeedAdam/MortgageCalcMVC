using MortgageCalcMVC.Models;

namespace MortgageCalcMVC.Helpers
{
    public class LoanHelper
    {
        public static Loan GetPayments(Loan loan)
        {
            // Calculate My Monthly Payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            //Create loop from 1 to the term
            //Calculate a payment schedule
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            //loop over each month until we reach the term of the loan
            for (int month = 1; month <= loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                //push payments into the loan
                loan.Payments.Add(loanPayment);
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = totalInterest + loan.Amount;

            //return the loan to the view
            return loan;
        }

        private static decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            decimal monthlyRate = CalcMonthlyRate(rate);
            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }


        private static decimal CalcMonthlyRate(decimal rate)
        {

            return rate / 1200;
        }

        private static decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }


    }
}
