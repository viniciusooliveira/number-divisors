import React from 'react';
import './App.css';
import getFactors from './services/factorService';
import Factor from './models/factor';

class App extends React.Component<{}, {
  factors: string,
  primes: string,
  number: string
}> {
  constructor(props: any){
    super(props);

    this.state = {
      factors: "",
      primes: "",
      number: ""
    }

    this.handleNumberChange = this.handleNumberChange.bind(this);
  }

  handleNumberChange(event: any){
    this.setState({...this.state, number: event.target.value});
  }

  async fetchFactors(){
    if(!this.validate())return;

    this.setState({...this.state, factors: "", primes: ""});

    try{
      getFactors.getFactors(parseInt(this.state.number), (factor: Factor) => {

        if(factor.number === 1){
          this.setState({...this.state,
            factors: factor.number.toString(),
            primes: factor.number.toString()})
        }else{
          this.setState({...this.state, factors: `${this.state.factors}, ${factor.number}`});

          if(factor.isPrime){
            this.setState({...this.state, primes: `${this.state.primes}, ${factor.number}`});
          }
        }
      });
    }catch (e) {
      alert("Erro ao calcular divisores.")
    }
    
  }

  validate(): boolean{
    if(this.state.number === "" || !this.state.number){
      alert("Insira um número.");
      return false;
    }
    try{
      const number = parseInt(this.state.number);

      if(number < 1){
        alert("O número deve ser maior ou igual a 1.");
        return false;
      }
      if(number > 2147483647){
        alert("O número deve ser menor ou igual a 2147483647.");
        return false;
      }
    }catch (e){
      alert("Insira um número válido!");
      return false;
    }
    return true;
  }

  render(){
    return (
    <div className="App">
      <header className="App-header">
        <h2>Cálculo de Divisores</h2>
        <input value={this.state.number} onChange={this.handleNumberChange} type="number" max="2147483647" min="1" placeholder="Insira um número" />
        <button onClick={() => this.fetchFactors()}>Calcular Divisores</button>
        <p>Números Divisores: <span>{this.state.factors}</span></p>
        <p>Divisores Primos: <span>{this.state.primes}</span></p>
      </header>
    </div>
  );
  }
}

export default App;
