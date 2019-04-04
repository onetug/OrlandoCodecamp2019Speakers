import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/BankTransactions';

class BankTransactions extends Component {
    componentDidMount() {
      
      // This method runs when the component is first added to the page
      this.props.requestBankTransactions();
    }
  
    render() {
      return (
      <div>
        <h1>Current Transactions</h1>
        <button disabled={this.props.isLoading} onClick={this.props.requestBankTransactions}>Refresh</button>

        <h2>Current Balance: {this.props.currentBalance}</h2>
        {this.renderTransactionsTable(this.props)}
    </div>);
    }

    renderTransactionsTable(props) {
      return (
        <table className='table'>
          <thead>
            <tr>
              <th>Date</th>
              <th>Transaction Type</th>
              <th>Amount</th>
              {/*<th>Summary</th>*/}
            </tr>
          </thead>
          <tbody>
            {this.renderMapTransactions(this.props.transactions)}
          </tbody>
        </table>
      );
    }

    renderMapTransactions(txs) {
      if(!txs) return [];

      return txs.map(tx => this.renderTransactionRow(tx));
    }

    renderTransactionRow(tx) {
      return (<tr key={tx.id}>
                <td>{tx.dateFormatted}</td>
                <td>{tx.type}</td>
                <td>{tx.amount}</td>
                {/*<td>{forecast.summary}</td>*/}
              </tr>);
    }

  }
  


export default connect(
  state => state.bankTransactions,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(BankTransactions);
