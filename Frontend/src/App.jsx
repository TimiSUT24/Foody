import {Routes, Route} from 'react-router-dom'
import HomePage from './Pages/HomePage'
import NavBar from './Components/NavBar'
import './App.css'

function App() {

  return (
    <>
    <NavBar></NavBar>
      <Routes>
        <Route path="/" element={<HomePage />}></Route>
      </Routes>
    </>
  )
}

export default App
