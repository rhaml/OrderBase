import './App.css'
import { OrderForm } from './components/OrderForm'

function App() {

  return (
    <div className='container'>
      <header className='header'>
        <h2>Ordem - Exposição financeira - Desafio</h2>
        <p>Order Generator</p>
      </header>
      <OrderForm/>
    </div>
  )
}

export default App
