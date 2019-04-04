import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Withdrawal';

class Withdrawal extends React.Component {
  
  constructor(props)
  {
    super(props);

    this.state = {
      withdrawalAmount: 0.00,
    };
    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event){
    var value = event.target.value;
    this.setState({withdrawalAmount: value});
  }

  render() {
    return (
      <div>
        <h1>Withdrawal Amount</h1>
    
        <p>$<input type="number" step="0.01" min="0" value={this.state.withdrawalAmount} onChange={this.handleChange}/></p>
    
        <button onClick={() => this.props.submitWithdrawal(this.state.withdrawalAmount)}>Withdraw</button>
      </div>
    );
  }
}

export default connect(
  state => state.withdrawal,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Withdrawal);
