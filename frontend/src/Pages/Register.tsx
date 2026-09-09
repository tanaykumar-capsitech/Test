import { Card } from "antd"
import axios from "axios"
import { useFormik } from "formik"

interface RegisterProps {
    name: string,
    email: string,
    password: string
}

const Register = () => {
    const RegisterUser = async (values: RegisterProps) => {
        const result = await axios.post('http://localhost:5129/API/Auth/register', values)
    }

    const initial: RegisterProps = {
        name: '',
        email: '',
        password: ''
    }

    const formik = useFormik({
        initialValues: initial,
        onSubmit: (value) => { RegisterUser(value) }
    })

    return (
        <>
            <Card style={{ margin: 'auto', marginTop: 300, width: 400 }} title="Register">
                <div className="text-left">
                    <form onSubmit={formik.handleSubmit}>
                        <label>Name</label> <br />
                        <input name="name" type="text" onChange={formik.handleChange} value={formik.values.name} className="px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input><br />
                        <label>Email</label> <br />
                        <input name="email" type="text" onChange={formik.handleChange} value={formik.values.email} className="px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input><br />
                        <label>Password</label> <br />
                        <input name="password" type="text" onChange={formik.handleChange} value={formik.values.password} className="px-2 text-[14px] border border-gray-300 outline-none rounded-md"></input><br />
                        <button type="submit" className="mt-3 px-2 border border-gray-300 bg-gray-100 hover:bg-gray-300 rounded-md">Register</button>
                    </form>
                </div>
            </Card>
        </>
    )
}

export default Register