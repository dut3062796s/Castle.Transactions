﻿using System;
using System.Diagnostics.Contracts;
using TransactionException = Castle.Services.Transaction.Exceptions.TransactionException;

namespace Castle.Services.Transaction
{
	[ContractClassFor(typeof (ITransaction))]
	internal abstract class ITransactionContract : ITransaction
	{
		string ITransaction.LocalIdentifier
		{
			[Pure]
			get
			{

				Contract.Requires(State != TransactionState.Disposed);
				Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
				throw new NotImplementedException();
			}
		}

		void ITransaction.Rollback()
		{
			Contract.Ensures(State == TransactionState.Aborted);
		}

		void ITransaction.Complete()
		{
			Contract.Requires(State == TransactionState.Active);
			// ->
			Contract.Ensures(State == TransactionState.CommittedOrCompleted
			                 || State == TransactionState.Aborted
			                 || State == TransactionState.InDoubt);

			Contract.EnsuresOnThrow<TransactionException>(
				State == TransactionState.Aborted);
		}

		void ITransaction.Dispose()
		{
			Contract.Requires(State == TransactionState.Active
			                  || State == TransactionState.Active
			                  || State == TransactionState.Aborted
			                  || State == TransactionState.InDoubt
			                  || State == TransactionState.CommittedOrCompleted);

			Contract.Ensures(State == TransactionState.Disposed);
		}

		public TransactionState State
		{
			get { return Contract.Result<TransactionState>(); }
		}

		public ITransactionOptions CreationOptions
		{
			get { throw new NotImplementedException(); }
		}

		public System.Transactions.Transaction Inner
		{
			get { throw new NotImplementedException(); }
		}

		Maybe<SafeKernelTxHandle> ITransaction.TxFHandle
		{
			get
			{
				Contract.Ensures(Contract.Result<Maybe<SafeKernelTxHandle>>() != null);
				throw new NotImplementedException();
			}
		}

		Maybe<IRetryPolicy> ITransaction.FailedPolicy
		{
			[Pure]
			get
			{
				var result = Contract.Result<Maybe<IRetryPolicy>>();
				Contract.Ensures(result != null);
				return result;
			}
		}

		void IDisposable.Dispose()
		{
		}
	}
}