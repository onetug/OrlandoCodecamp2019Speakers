using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Domain;
using frontend.Data;

namespace frontend.Logic.DomainEvents
{
    public class AccountAggregate : AggregateRoot
    {
        public Guid AccountId { get; private set; }
        public decimal CurrentBalance { get; private set; } = 0;
        public bool IsAccountOpen { get; private set; } = false;

        private AccountAggregate() : base()
        {}

        public static AccountAggregate OpenNewAccount(Guid accountId)
        {
            var account = new AccountAggregate();
            // normally opening a new account we would be generating this in another service
            account.Id = accountId;
            account.ApplyChange(new AccountOpenedEvent(accountId));
            return account;
        }

        private void Apply(AccountOpenedEvent @event)
        {
            AccountId = @event.AccountId;
            IsAccountOpen = true;
            CurrentBalance = 0;
        }

        public void Deposit(Deposit deposit)
        {
            if(!IsAccountOpen)
            {
                throw new ApplicationException("Account is not open");
            }

            var depositedEvent = new AmountDepositedEvent(
                    deposit.DepositId,
                    deposit.Amount,
                    deposit.Date,
                    deposit.AccountId);

            ApplyChange(depositedEvent);
        }

        private void Apply(AmountDepositedEvent @event)
        {
            CurrentBalance += @event.Amount;
        }

        public void Withdraw(Withdrawal withdrawal)
        {
            if(!IsAccountOpen)
            {
                throw new ApplicationException("Account is not open");
            }

            var withdrawnEvent = new AmountWithdrawnEvent(
                    withdrawal.WithdrawalId,
                    withdrawal.Amount,
                    withdrawal.Date,
                    withdrawal.AccountId);

            ApplyChange(withdrawnEvent);
        }

        private void Apply(AmountWithdrawnEvent @event)
        {
            CurrentBalance -= @event.Amount;
        }
    }
}