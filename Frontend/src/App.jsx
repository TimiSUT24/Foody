import {Routes, Route} from 'react-router-dom'
import HomePage from './Pages/HomePage'
import NavBar from './Components/NavBar'
import CartPage from './Pages/CartPage'
import DetailsPage from './Pages/ProductDetailsPage'
import './App.css'


function App() {

  return (
    <>
    <NavBar></NavBar>
      <Routes>
        <Route path="/" element={<HomePage />}></Route>
        <Route path="/cart" element={<CartPage />}></Route>
        <Route path="/product/:id" element={<DetailsPage />}></Route>
      </Routes>
    </>
  )
}

export default App
