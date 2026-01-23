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
import OrderThankYouPage from './Pages/OrderThankYouPage'
import AboutPage from './Pages/AboutPage'
import UserProtectedRoute from './Components/UserProtectedRoute'
import './App.css'

function App() {

  return (
    <>
    <NavBar></NavBar>

        <Routes> 
          {/* Public */}
        <Route path="/" element={<HomePage />}></Route>
        <Route path="/about" element={<AboutPage />}></Route>
        <Route path="/product/:id" element={<DetailsPage />}></Route>
        <Route path="/login" element={<LoginPage />}></Route>
        <Route path="/register" element={<RegisterPage />}></Route>
        

        <Route element={<UserProtectedRoute roles={["User"]} />}>
            {/* User */}
          <Route path="/cart" element={<CartPage />}></Route>
          <Route path="/order-complete" element={<CompletePage />}></Route>
          <Route path="/user-page" element={<UserPage />}></Route>
          <Route path="/user-profile-page" element={<UserProfilePage />}></Route>
          <Route path="/thank-you-page" element={<OrderThankYouPage />}></Route>
        </Route>
        
      </Routes>  
        
    </>
  )
}

export default App
