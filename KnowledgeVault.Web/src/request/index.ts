import axios from 'axios'
import { ElMessage } from 'element-plus'
import baseUrl from './url'
import Cookies from 'js-cookie'

// 创建instance实例
const instance = axios.create({
    baseURL: baseUrl,
    timeout: 5000,
    headers: {
        "Content-Type": "application/json",
        'Cache-Control': 'max-age=3600'
    }
})

// 请求拦截
instance.interceptors.request.use((config) => {
    const token=Cookies.get("token")
    console.log(token)
    if(token){
        config.headers.Authorization = token
    }
    return config
})


// 响应拦截
instance.interceptors.response.use((res) => {
    // if (res.status !== 200) {
    //     return false
    // }
    return res.data

}, (error) => {
    console.log(error)
    switch (error.response.status) {
        case 400:
            ElMessage.error(error.response.data.title)
            break
        case 401:
            ElMessage.error(error.response.data)
            break
        case 409:
            ElMessage.error(error.response.data)
            break
        default:
            console.log('未知错误')
    }
})

export default instance;