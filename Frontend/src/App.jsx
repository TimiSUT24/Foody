import {Routes, Route} from 'react-router-dom'
import HomePage from './Pages/HomePage'
import NavBar from './Components/NavBar'
import CartPage from './Pages/CartPage'
import DetailsPage from './Pages/ProductDetailsPage'
import LoginPage from './Pages/LoginPage'
import RegisterPage from './Pages/RegisterPage'
import CompletePage from './Pages/CompletePage'
import UserPage from './Pages/User/UserPage'
import UserProfilePage from './Pages/User/UserProfilePage'
import './App.css'


function App() {


  return (
    <>
    <NavBar></NavBar>
        <Routes> 
        <Route path="/" element={<HomePage />}></Route>
        <Route path="/cart" element={<CartPage />}></Route>
        <Route path="/product/:id" element={<DetailsPage />}></Route>
        <Route path="/login" element={<LoginPage />}></Route>
        <Route path="/register" element={<RegisterPage />}></Route>
        <Route path="/order-complete" element={<CompletePage />}></Route>
        <Route path="/user-page" element={<UserPage />}></Route>
        <Route path="/user-profile-page" element={<UserProfilePage />}></Route>
      </Routes>  



        
    </>
  )
}

export default App
