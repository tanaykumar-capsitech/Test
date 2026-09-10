import { LoadingOutlined } from "@ant-design/icons"
import { Card, Spin } from "antd"
import axios from "axios"
import { useFormik } from "formik"
import { useState } from "react"
import { Link, useNavigate } from "react-router-dom"

const url = import.meta.env.VITE_BASE_URL

interface LoginProps {
    email: string,
    password: string
}

const Login = () => {
    const [error, setError] = useState("")
    const [loading, setLoading] = useState(false)
    const navigate = useNavigate()

    const LoginUser = async (values: LoginProps) => {
        const response = await axios.post(url + 'Auth/login', values, { withCredentials: true })
        setLoading(false)

        if (response.data.statusCode != 201) {
            setError(response.data.message)

            return
        }

        setError("")
        navigate("/")
    }

    const initial: LoginProps = {
        email: '',
        password: ''
    }

    const formik = useFormik({
        initialValues: initial,
        onSubmit: (value) => {
            setLoading(true)
            LoginUser(value)
        }
    })

    return (
        <>
            <Card className="shadow-xl" style={{ margin: 'auto', marginTop: 300, width: '35vw', maxWidth: 350, minWidth: 300, borderRadius: 20 }} title="Login">
                <form onSubmit={formik.handleSubmit} className="mb-2">
                    <div className="mb-2 flex justify-between items-center">
                        <label>Email</label>
                        <input name="email" type="email" onChange={formik.handleChange} value={formik.values.email} className="hover:shadow-lg px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input>
                    </div>
                    <div className="mb-2 flex justify-between items-center">
                        <label>Password</label>
                        <input name="password" type="text" onChange={formik.handleChange} value={formik.values.password} className="hover:shadow-lg px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input>
                    </div>
                    <div className="text-[12px] text-red-600">{error}</div>
                    <button type="submit" className="w-20 transition-all duration-300 mt-3 px-2 border border-gray-300 rounded-md hover:bg-gray-100 hover:shadow-lg">{loading ? <Spin indicator={<LoadingOutlined spin />} size="small"></Spin> : 'Login'}</button>
                </form>
                Don't have an account? <Link to="/register">Register</Link>
            </Card>
        </>
    )
}

export default Login