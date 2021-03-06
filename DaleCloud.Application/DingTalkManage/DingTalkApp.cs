﻿/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: DaleCloud
 * Description: DaleCloud快速开发平台
 * Website：
*********************************************************************************/

using System.Collections.Generic;
using System;
using System.Linq;
using DaleCloud.DingTalk;
using DaleCloud.Code;
using DaleCloud.Entity.DingTalk;
using DaleCloud.Repository.DingTalk;

namespace DaleCloud.Application.DingTalk
{
    public class DingTalkApp
    {
        private DingTalkCorpConfigRepository service = new DingTalkCorpConfigRepository();
        private DingTalkAppConfigRepository app = new DingTalkAppConfigRepository();
        public List<DingTalkCorpConfigEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<DingTalkCorpConfigEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.AppKey.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.uuId).ToList();
        }
        public DingTalkCorpConfigEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.uuId == keyValue);
        }

        public void SubmitForm(DingTalkCorpConfigEntity roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                service.Update(roleEntity);
            }
            else
            {
                service.Insert(roleEntity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DingTalkCorpConfig GetCorpConfig()
        {
            DingTalkCorpConfigEntity entity = service.FindEntity(1);
            if (entity != null)
            {
                DingTalkCorpConfig config = new DingTalkCorpConfig();

                config.CorpId = entity.CorpId;
                config.CorpSecret = entity.CorpSecret;
                config.AgentId = entity.AgentId;
                config.AppName = entity.AppName;
                config.AppKey = entity.AppKey;
                config.AppSecret = entity.AppSecret;
                config.HeadPic = entity.HeadPic;
                return config;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DingTalkAppConfig GetAppConfig(string appcode)
        {
            DingTalkAppConfigEntity entity = app.FindEntity(appcode);
            if (entity != null)
            {
                DingTalkAppConfig config = new DingTalkAppConfig();
                
                config.AgentId = entity.AgentId;
                config.AppId = entity.AppId;
                config.AppName = entity.AppName;
                config.AppKey = entity.AppKey;
                config.AppSecret = entity.AppSecret;
                return config;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取GetAccessToken
        /// </summary>
        /// <returns></returns>
        public DingTalkCorpConfig GetAccessToken()
        {
            if (DingTalkHelper.CorpConfig != null)
            {
                if(string.IsNullOrEmpty(DingTalkHelper.CorpConfig.CorpId) || string.IsNullOrEmpty(DingTalkHelper.CorpConfig.CorpSecret))
                {
                    DingTalkHelper.CorpConfig = GetCorpConfig();
                    DingTalkHelper.CorpConfig.AccessToken = DingTalkHelper.GetAccessToken(DingTalkHelper.CorpConfig.CorpId, DingTalkHelper.CorpConfig.CorpSecret);
                    DingTalkHelper.CorpConfig.ExpiresTime = DateTime.Now;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - DingTalkHelper.AppConfig.ExpiresTime;
                    if (ts.TotalSeconds>= DingTalkHelper.CorpConfig.ExpiresIn)
                    {
                        DingTalkHelper.CorpConfig.AccessToken = DingTalkHelper.GetAccessToken(DingTalkHelper.CorpConfig.CorpId, DingTalkHelper.CorpConfig.CorpSecret);
                        DingTalkHelper.CorpConfig.ExpiresTime = DateTime.Now;
                    }
                }
                return DingTalkHelper.CorpConfig;
            }
            else
            {
                DingTalkHelper.CorpConfig = GetCorpConfig();
                DingTalkHelper.CorpConfig.AccessToken = DingTalkHelper.GetAccessToken(DingTalkHelper.CorpConfig.CorpId, DingTalkHelper.CorpConfig.CorpSecret);
                DingTalkHelper.CorpConfig.ExpiresTime = DateTime.Now;
                return DingTalkHelper.CorpConfig;
            }
        }
    }
}
