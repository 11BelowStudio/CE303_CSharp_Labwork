using System;
using System.Collections.Generic;

namespace BankServer
{
    class Bank
    {

        private static int NEXT_VALID_ACCOUNT = 1000;

        private static Object locker = new Object();

        private static int GenerateAccountNumber(int accountNumberToTest = 0)
        {
            int validAccountNumber = accountNumberToTest;
            lock(locker)
            {
                if (validAccountNumber < NEXT_VALID_ACCOUNT){
                    //if account number being tested is less than the next valid one,
                    //increment it so it's now valid.
                    validAccountNumber = NEXT_VALID_ACCOUNT;
                }
                else
                {
                    //if the account number being tested isn't smaller than the next valid one,
                    //make the next one equal to what ever this is
                    NEXT_VALID_ACCOUNT = validAccountNumber;
                }
                NEXT_VALID_ACCOUNT++; //increment the NEXT_VALID_ACCOUNT
            }
            return validAccountNumber;
        }

        private readonly Dictionary<int, Account> accounts = new Dictionary<int, Account>();

        public void CustomerCreateAccount(int customerId)
        {
            CreateAccount(customerId, 0, 0);
        }

        public void CreateAccount(int customerId, int accountNumber, int initialBalance)
        {
            accountNumber = GenerateAccountNumber(accountNumber);
            Account account = new Account(customerId, accountNumber);
            account.Balance = initialBalance;
            accounts.Add(accountNumber, account);
        }

        public int[] GetListOfAccounts(int customerId)
        {
            List<int> result = new List<int>();
            foreach (Account account in accounts.Values)
                if (account.CustomerId == customerId)
                    result.Add(account.AccountNumber);

            return result.ToArray();
        }

        public int GetAccountBalance(int customerId, int accountNumber)
        {
            if (accounts[accountNumber].CustomerId != customerId)
                throw new Exception($"Account {accountNumber} belongs to a different customer; customer {customerId} is not authorised to query balance for this account.");

            return accounts[accountNumber].Balance;
        }

        public void Transfer(int customerId, int fromAccount, int toAccount, int amount)
        {
            lock (accounts)
            {
                if (accounts[fromAccount].CustomerId != customerId)
                    throw new Exception($"Account {fromAccount} belongs to a different customer; customer {customerId} is not authorised to transfer from this account.");
                if (accounts[fromAccount].Balance < amount)
                    throw new Exception(
                        $"The balance of account {fromAccount} is {accounts[fromAccount].Balance} which is insufficient to transfer {amount}.");
                if (amount <= 0)
                    throw new Exception("Transfer amount has to be a positive value.");
                accounts[fromAccount].Balance -= amount;
                accounts[toAccount].Balance += amount;
            }
        }
    }
}
