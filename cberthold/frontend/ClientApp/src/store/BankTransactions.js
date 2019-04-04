const requestBankTransactionsType = 'REQUEST_BANK_TRANSACTIONS';
const receiveBankTransactionsType = 'RECEIVE_BANK_TRANSACTIONS';
const errorBankTransactionsType = 'ERROR_BANK_TRANSACTIONS';
const initialState = { currentBalance: 0.0, transactions: [], isLoading: false };
const accountNumber = "4ff8fae5-e2fe-4d65-9f59-cf95cb5f31ea";

export const actionCreators = {
  requestBankTransactions: () => async (dispatch, getState) => {    
    if (getState().isLoading) {
      // Don't issue a duplicate request (we already have or are loading the requested data)
      return;
    }

    dispatch({ type: receiveBankTransactionsType });

    const url = `api/Account/${accountNumber}/Transactions`;

    try
    {
    const response = await fetch(url);
    const data = await response.json();
    const transactions = data.transactions;
    const currentBalance = data.currentBalance;

    dispatch({ type: receiveBankTransactionsType, transactions, currentBalance });
    } catch(ex)
    {
      dispatch({ type: errorBankTransactionsType });
    }
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestBankTransactionsType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === receiveBankTransactionsType) {
    return {
      ...state,
      transactions: action.transactions,
      currentBalance: action.currentBalance,
      isLoading: false
    };
  }

  if (action.type === errorBankTransactionsType) {
    return {
      ...state,
      isLoading: false
    };
  } 

  return state;
};
