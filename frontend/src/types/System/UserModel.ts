/**
 * @description: Login interface parameters
 */
export interface LoginParams {
  user_name: string
  password: string
}

/**
 * @description: Login interface return value
 */
export interface LoginResultModel {
  userId: string | number
  token: string
}

/**
 * @description: Get user information return value
 */
// export interface GetUserInfoModel {
//   // 用户id
//   userId: string | number;
//   // 用户名
//   username: string;
//   // 真实名字
//   realName: string;
//   // 头像
//   avatar: string;
//   // 介绍
//   desc?: string;
// }
