import { Card } from "antd"
import axios from "axios"
import { useFormik } from "formik"
import { useState } from "react"
import { Link, useNavigate } from "react-router-dom"

const url = import.meta.env.VITE_BASE_URL

interface RegisterProps {
    name: string,
    email: string,
    password: string
}

const Register = () => {
    const [error, setError] = useState("")
    const navigate = useNavigate()
    
    const RegisterUser = async (values: RegisterProps) => {
        const response = await axios.post(url+'Auth/register', values)

        console.log(response.data)

        if (response.data.statusCode != 201){
            setError(response.data.message)

            return
        }

        setError("")
        navigate("/")
    }

    const initial: RegisterProps = {
        name: '',
        email: '',
        password: ''
    }

    const formik = useFormik({
        initialValues: initial,
        onSubmit: (value) => { 
            RegisterUser(value) 
        }
    })

    return (
        <>
            <Card className="shadow-xl" style={{ margin: 'auto', marginTop: 300, width: '35vw', maxWidth: 350, minWidth: 300 }} title="Register">
                <form onSubmit={formik.handleSubmit}>
                    <div className="mb-2 w-full flex justify-between items-center">
                        <label >Name</label>
                        <input name="name" type="text" onChange={formik.handleChange} value={formik.values.name} className="hover:shadow-lg px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input>
                    </div>
                    <div className="mb-2 flex justify-between items-center">
                        <label>Email</label>
                        <input name="email" type="email" onChange={formik.handleChange} value={formik.values.email} className="hover:shadow-lg px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input>
                    </div>
                    <div className="mb-2 flex justify-between items-center">
                        <label>Password</label>
                        <input name="password" type="text" onChange={formik.handleChange} value={formik.values.password} className="hover:shadow-lg px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input>
                    </div>
                    <div className="text-[12px] text-red-600">{error}</div>
                    <button type="submit" className="transition-all duration-300 mt-3 px-2 border border-gray-300 rounded-md hover:bg-gray-100 hover:shadow-lg">Register</button>
                </form>
                Already have an account? <Link to="/">Login</Link>
            </Card>
        </>
    )
}

export default Register