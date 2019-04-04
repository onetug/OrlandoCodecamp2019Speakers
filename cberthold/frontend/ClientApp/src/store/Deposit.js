const submitDepositType = 'SUBMIT_DEPOSIT';
const submitSuccessType = 'DEPOSIT_SUCCESS';
const initialState = { isLoading: false };
const accountNumber = "4ff8fae5-e2fe-4d65-9f59-cf95cb5f31ea";

export const actionCreators = {
  submitDeposit: (depositAmount) => async (dispatch, getState) => {    
    if (getState().isLoading) {
      // Don't issue a duplicate request (we already have or are loading the requested data)
      return;
    }

    dispatch({ type: submitDepositType });

    var parsedDepositAmount = parseFloat(parseFloat(depositAmount).toFixed(2));

    const data = {
      amount: parsedDepositAmount,
    };

    const url = `api/Account/${accountNumber}/Deposit`;
    
    const response = await fetch(url, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, cors, *same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "same-origin", // include, *same-origin, omit
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data), // body data type must match "Content-Type" header
    });

    dispatch({ type: submitSuccessType });
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === submitDepositType) {
    return {
      ...state,
      isLoading: true
    };
  }

  if (action.type === submitSuccessType) {
    return {
      ...state,
      isLoading: false
    };
  }

  return state;
};
