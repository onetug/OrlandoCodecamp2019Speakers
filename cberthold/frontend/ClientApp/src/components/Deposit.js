import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/Deposit';

class Deposit extends React.Component {
  
  constructor(props)
  {
    super(props);

    this.state = {
      depositAmount: 0.00,
    };
    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event){
    var value = event.target.value;
    this.setState({depositAmount: value});
  }

  render() {
    return (
      <div>
        <h1>Deposit Amount</h1>
    
        <p>$<input type="number" step="0.01" min="0" value={this.state.depositAmount} onChange={this.handleChange}/></p>
    
        <button onClick={() => this.props.submitDeposit(this.state.depositAmount)}>Deposit</button>
      </div>
    );
  }
}

export default connect(
  state => state.deposit,
  dispatch => bindActionCreators(actionCreators, dispatch)
)(Deposit);
