import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import BankTransactions from './components/BankTransactions';
import Deposit from './components/Deposit';
import Withdrawal from './components/Withdrawal';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/transactions' component={BankTransactions} />
    <Route path='/deposit' component={Deposit} />
    <Route path='/withdrawal' component={Withdrawal} />
  </Layout>
);
